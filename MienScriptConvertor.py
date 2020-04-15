# -*- coding: utf-8 -*-
"""
Created on Sun Apr 12 02:06:20 2020

@author: LERTSIRIKARN

EXAMPLE
{"from":"MienThai",
"to":"MienNewRoman",
"text":"ก๊าน เจี๊ยน มี่ง อ๊ะ"}

"""
from lstm_seq_to_seq_restore import RNN_convertor
#import pandas as pd
from flask import Flask, jsonify, request
#import pickle
#import json
from xml.etree import ElementTree
#import io
import re


def search_node(from_script,to_script,input_text,root_node):
    
    for word in root_node:

        current_node_word=word.find(from_script)
        if current_node_word is not None:
            if current_node_word.text == input_text:
                current_equivalet_word=word.find(to_script)
                
                if current_equivalet_word is not None:
                    return current_equivalet_word.text
            


def convert(from_script,to_script,input_text):
    
    script_name_dict = {
      "MienThai": "thaiscript",
      "MienNewRoman": "newromanscript",
      "MienOldRoman": "oldromanscript",
      "MienLao": "laoscript"
    }
    from_script_XML_name=script_name_dict[from_script]
    to_script_XML_name=script_name_dict[to_script]
  
    word_input=input_text
    return_string =''
    
    # get the xml with word equivalent
    ScriptMatchRootDocument = ElementTree.parse('MienWordsEquivalent/ScriptEquivalentData.xml')
    
    ScriptMatchRoot = ScriptMatchRootDocument.getroot()
    
    #special char to split upon
    reg_ex_match='([.\[\{\(\)\}\]\,\;\?\!\s])'
    data_path='DataSet/'+from_script+'To'+to_script+'DataSet.txt'
    model_path='SavedModel/'+from_script+'To'+to_script+'_s2s.h5'

    # initialize the model for predicting word equvalent
    RNNconvertor=RNN_convertor(data_path, model_path)
    splitted_input=re.split(reg_ex_match, word_input)
    for single_word in splitted_input:
		print(single_word)
		print(return_string)
        if single_word == '':
            continue
        
        if re.match(reg_ex_match, single_word): 
            # these are special char, so we can skip trasnlating it
            return_string=return_string+single_word
            continue
            
        else:
            transliterated_return_XML=search_node(from_script_XML_name,to_script_XML_name,input_text,ScriptMatchRoot)
            if transliterated_return_XML is not None:

                
                return_string=return_string+transliterated_return_XML
                continue
            else:
                
                # try to search in the XML again with lowercase,remove ^ -
                single_word_remove_carat=single_word.replace("^", "")
                single_word_remove_dash=single_word_remove_carat.replace("-", "")
                single_word=single_word_remove_dash
                
                if(from_script == 'MienOldRoman' or from_script=='MienNewRoman'):
                    single_word_lower=single_word_remove_dash.lower()
                    single_word=single_word_lower
                
                #search again
                transliterated_return_XML_2=search_node(from_script_XML_name,to_script_XML_name,input_text,ScriptMatchRoot)
                if transliterated_return_XML_2 is not None:
                    return_string=return_string+transliterated_return_XML_2
                    continue

                # try transcribing using SeqToSeq RNN
                try:

                    transliterated_return_RNN = RNNconvertor.transliterate_predict(single_word)
                    transliterated_return_RNN=transliterated_return_RNN.rstrip('\n')

                # if we also cannot use Sequence to sequence then just return the original string
                except:
                    transliterated_return_RNN = single_word
                return_string=return_string+transliterated_return_RNN

    # return data
    return (return_string)

# Test
#word_input="หล,ง (ss); เยย?\nll!?\n"
#result=convert("MienThai","MienNewRoman",word_input)


