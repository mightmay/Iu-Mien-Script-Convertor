'''
#Restore a character-level sequence to sequence model from to generate predictions.

This script loads the ```s2s.h5``` model saved by [lstm_seq2seq.py
](/examples/lstm_seq2seq/) and generates sequences from it. It assumes
that no changes have been made (for example: ```latent_dim``` is unchanged,
and the input data and model architecture are unchanged).

See [lstm_seq2seq.py](/examples/lstm_seq2seq/) for more details on the
model architecture and how it is trained.

modified from:
    https://python3.wannaphong.com/2018/09/thai-romanization-keras-python.html
'''
from __future__ import print_function

from keras.models import Model, load_model
from keras.layers import Input
import numpy as np

#data_path='DataSet/MienThaiToMienNewRomanDataSet.txt'
#model_path='SavedModel/MienThaiToMienNewRoman_s2s.h5'
class RNN_convertor:
    def __init__(self,data_path,model_path):
        self.batch_size = 64  # Batch size for training.
        self.epochs = 100  # Number of epochs to train for.
        latent_dim = 256  # Latent dimensionality of the encoding space.
        num_samples = 10000  # Number of samples to train on.
        # Path to the data txt file on disk.
        data_path = data_path
        
        # Vectorize the data.  We use the same approach as the training script.
        # NOTE: the data must be identical, in order for the character -> integer
        # mappings to be consistent.
        # We omit encoding target_texts since they are not needed.
        input_texts = []
        target_texts = []
        input_characters = set()
        target_characters = set()
        with open(data_path, 'r', encoding='utf-8') as f:
            lines = f.read().split('\n')
        for line in lines[: min(num_samples, len(lines) - 1)]:
            input_text, target_text = line.split('\t')
            # We use "tab" as the "start sequence" character
            # for the targets, and "\n" as "end sequence" character.
            target_text = '\t' + target_text + '\n'
            input_texts.append(input_text)
            target_texts.append(target_text)
            for char in input_text:
                if char not in input_characters:
                    input_characters.add(char)
            for char in target_text:
                if char not in target_characters:
                    target_characters.add(char)
        
        input_characters = sorted(list(input_characters))
        target_characters = sorted(list(target_characters))
        self.num_encoder_tokens = len(input_characters)
        self.num_decoder_tokens = len(target_characters)
        self.max_encoder_seq_length = max([len(txt) for txt in input_texts])
        self.max_decoder_seq_length = max([len(txt) for txt in target_texts])
        
        print('Number of samples:', len(input_texts))
        print('Number of unique input tokens:', self.num_encoder_tokens)
        print('Number of unique output tokens:', self.num_decoder_tokens)
        print('Max sequence length for inputs:', self.max_encoder_seq_length)
        print('Max sequence length for outputs:', self.max_decoder_seq_length)
        
        self.input_token_index = dict(
            [(char, i) for i, char in enumerate(input_characters)])
        self.target_token_index = dict(
            [(char, i) for i, char in enumerate(target_characters)])
        
        encoder_input_data = np.zeros(
            (len(input_texts), self.max_encoder_seq_length, self.num_encoder_tokens),
            dtype='float32')
        
        for i, input_text in enumerate(input_texts):
            for t, char in enumerate(input_text):
                encoder_input_data[i, t, self.input_token_index[char]] = 1.
        
        # Restore the model and construct the encoder and decoder.
        try:
            model = load_model(model_path)
            
            # if train on different Keras version
        except:
            import h5py
            print("using Keras 2.3 saved model")
            f_2 = h5py.File(model_path,'r+')
            data_p = f_2.attrs['training_config']
            data_p = data_p.decode().replace("learning_rate","lr").encode()
            f_2.attrs['training_config'] = data_p
            f_2.close()
            model = load_model(model_path)
            
        encoder_inputs = model.input[0]   # input_1
        encoder_outputs, state_h_enc, state_c_enc = model.layers[2].output   # lstm_1
        encoder_states = [state_h_enc, state_c_enc]
        self.encoder_model = Model(encoder_inputs, encoder_states)
        
        decoder_inputs = model.input[1]   # input_2
        decoder_state_input_h = Input(shape=(latent_dim,), name='input_3')
        decoder_state_input_c = Input(shape=(latent_dim,), name='input_4')
        decoder_states_inputs = [decoder_state_input_h, decoder_state_input_c]
        decoder_lstm = model.layers[3]
        decoder_outputs, state_h_dec, state_c_dec = decoder_lstm(
            decoder_inputs, initial_state=decoder_states_inputs)
        decoder_states = [state_h_dec, state_c_dec]
        decoder_dense = model.layers[4]
        decoder_outputs = decoder_dense(decoder_outputs)
        self.decoder_model = Model(
            [decoder_inputs] + decoder_states_inputs,
            [decoder_outputs] + decoder_states)
        
        # Reverse-lookup token index to decode sequences back to
        # something readable.
        self.reverse_input_char_index = dict(
            (i, char) for char, i in self.input_token_index.items())
        self.reverse_target_char_index = dict(
            (i, char) for char, i in self.target_token_index.items())


    # Decodes an input sequence.  Future work should support beam search.
    def decode_sequence(self,input_seq):
        # Encode the input as state vectors.
        states_value = self.encoder_model.predict(input_seq)

        # Generate empty target sequence of length 1.
        target_seq = np.zeros((1, 1, self.num_decoder_tokens))

        # Populate the first character of target sequence with the start character.
        target_seq[0, 0, self.target_token_index['\t']] = 1.

        # Sampling loop for a batch of sequences
        # (to simplify, here we assume a batch of size 1).
        stop_condition = False
        decoded_sentence = ''
        while not stop_condition:
            output_tokens, h, c = self.decoder_model.predict(
                [target_seq] + states_value)


            # Sample a token
            sampled_token_index = np.argmax(output_tokens[0, -1, :])
            sampled_char = self.reverse_target_char_index[sampled_token_index]
            decoded_sentence += sampled_char
    
            # Exit condition: either hit max length
            # or find stop character.
            if (sampled_char == '\n' or
               len(decoded_sentence) > self.max_decoder_seq_length):
                stop_condition = True
    
            # Update the target sequence (of length 1).
            target_seq = np.zeros((1, 1, self.num_decoder_tokens))
            target_seq[0, 0, sampled_token_index] = 1.
    
            # Update states
            states_value = [h, c]
        
        #print("used LSTM seq to seq" )
    
        return decoded_sentence
    
    def encode_input(self,word):

        test_input = np.zeros(
            (1, self.max_encoder_seq_length, self.num_encoder_tokens),
            dtype='float32')
        for t, char in enumerate(word):
            test_input[0, t, self.input_token_index[char]] = 1.
        return test_input
    
    def transliterate_predict(self,text):
        #print("s2s")
        #print("s2s in="+text)
        encoded_input=self.encode_input(text)
        return_str=self.decode_sequence(encoded_input)


        return return_str
    
    #for seq_index in range(100):
        # Take one sequence (part of the training set)
        # for trying out decoding.
    #    input_seq = encoder_input_data[seq_index: seq_index + 1]
    #    decoded_sentence = decode_sequence(input_seq)

    #   print('Input sentence:', input_texts[seq_index])
    #    print('Decoded sentence:', decoded_sentence)
        

