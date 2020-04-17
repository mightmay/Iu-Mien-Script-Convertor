# -*- coding: utf-8 -*-
"""
Created on Sun Apr 12 02:06:20 2020

@author: LERTSIRIKARN

POST EXAMPLE
{"from":"mienthai",
"to":"newroman",
"text":"ก๊าน เจี๊ยน มี่ง อ๊ะ"}

"""

#import pandas as pd
from flask import Flask, request
#from flask import jsonify
#import pickle
#import json
# load model
from MienScriptConvertor import convert

# app
app = Flask(__name__)

# routes
@app.route('/', methods=['GET', 'POST'])

def predict():


    if request.method=='POST':
        input_script =request.form['from']
        output_script=request.form['to']
        text_to_transliterate= request.form['text']
    if request.method=='GET':
        input_script =request.args.get('from')
        output_script=request.args.get('to')
        text_to_transliterate= request.args.get('text')    
        
    transliterated_return = convert(input_script,output_script,text_to_transliterate)
    
    # return data
    return (transliterated_return)

if __name__ == '__main__':
    
    app.run(port = 5000, debug=True)
    #app.run(port = 5000,debug=True, use_reloader=False)