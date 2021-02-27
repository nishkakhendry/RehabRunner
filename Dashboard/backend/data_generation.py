import pymongo
from pymongo import MongoClient
from random import randint

client = MongoClient(
    'mongodb+srv://admin:admin@workshopcluster.xwsg8.mongodb.net/myFirstDatabase?retryWrites=true&w=majority')
col = client['maybekinnectgame']['userdata']


days = ['Monday', 'Tuesday', 'Wednesday',
        'Thursday', 'Friday', 'Saturday', 'Sunday']


def generate_random_data_curr_week():
    result = dict()
    for i in days:
        r = randint(1, 100)
        if r < 50:
            r = 0
        result[i] = r

    return {
        "currweek": result
    }


def generate_doc_time():
    return {
        "doc_time": randint(1, 2)
    }


def generate_average_score():
    return {
        "average_score": randint(40, 70)
    }


def generate_name():
    from faker import Faker
    fake = Faker()
    return {
        "name": fake.name()
    }


for i in range(1000):
    post = {
        **generate_random_data_curr_week(),
        **generate_doc_time(),
        **generate_average_score(),
        **generate_name(),
    }
    # print(post)
    col.insert_one(post)
