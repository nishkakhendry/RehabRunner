from flask import Flask, jsonify
from flask_cors import CORS
import pymongo
from pymongo import MongoClient
from bson.objectid import ObjectId
import logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger()

client = MongoClient(
    'mongodb+srv://admin:admin@workshopcluster.xwsg8.mongodb.net/myFirstDatabase?retryWrites=true&w=majority')
col = client['maybekinnectgame']['userdata']

app = Flask(__name__)
CORS(app)


@app.route('/')
def hello():
    return "Hello World!"


@app.route('/user/<userid>')
def user(userid):
    try:
        result = col.find_one({"_id": ObjectId(userid)})
        print(result)
        result["_id"] = str(result["_id"])
        return jsonify(result)
    except:
        logger.exception("user exception")
        return "none"


if __name__ == '__main__':
    app.run(port=3000)
