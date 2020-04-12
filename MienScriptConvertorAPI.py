# -*- coding: utf-8 -*-
"""
Created on Sun Apr 12 02:06:20 2020

@author: LERTSIRIKARN

POST EXAMPLE
{"from":"mienthai",
"to":"newroman",
"text":"ก๊าน เจี๊ยน มี่ง อ๊ะ"}

"""
from MienThaiToMienNewRoman_lstm_seq2seq_restore import ThaiToNewRoman
import pandas as pd
from flask import Flask, jsonify, request
import pickle
import json
# load model


# app
app = Flask(__name__)

# routes
@app.route('/', methods=['POST'])

def predict():
    # get data
    json_data = request.get_json(force=True)

    # convert data into dict

    input_script =json_data["from"]
    output_script=json_data["to"]
    text_to_transliterate= json_data["text"]

    transliterated_return = ThaiToNewRoman(text_to_transliterate)
    # send back to browser
    output = {'from':input_script,'to':output_script,'input_text':text_to_transliterate,'transliterated':transliterated_return}

    # return data
    return jsonify(results=output)

if __name__ == '__main__':
    app.run(port = 5000, debug=True)