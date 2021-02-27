import cv2 as cv
import numpy as np
import argparse
import time
from concurrent.futures import ThreadPoolExecutor
from collections import deque
import logging
from flask import Flask
from celery import Celery
import socket
from io import BytesIO

logging.basicConfig(level=logging.DEBUG)
logger = logging.getLogger()
logger.handlers = []
# fh = logging.FileHandler("points.log")
fh = logging.StreamHandler()
fh.setLevel(logging.INFO)
logger.addHandler(fh)

THRESHOLD = 0.1
BODY_PARTS = {"Nose": 0, "Neck": 1, "RShoulder": 2, "RElbow": 3, "RWrist": 4,
              "LShoulder": 5, "LElbow": 6, "LWrist": 7, "RHip": 8, "RKnee": 9,
              "RAnkle": 10, "LHip": 11, "LKnee": 12, "LAnkle": 13, "REye": 14,
              "LEye": 15, "REar": 16, "LEar": 17, "Background": 18}

BODY_PARTS_INVERSE = ["" for i in range(len(BODY_PARTS))]
for key in BODY_PARTS:
    index = BODY_PARTS[key]
    BODY_PARTS_INVERSE[index] = key

POSE_PAIRS = [["Neck", "RShoulder"], ["Neck", "LShoulder"], ["RShoulder", "RElbow"],
              ["RElbow", "RWrist"], ["LShoulder",
                                     "LElbow"], ["LElbow", "LWrist"],
              ["Neck", "RHip"], ["RHip", "RKnee"], [
    "RKnee", "RAnkle"], ["Neck", "LHip"],
    ["LHip", "LKnee"], ["LKnee", "LAnkle"], [
    "Neck", "Nose"], ["Nose", "REye"],
    ["REye", "REar"], ["Nose", "LEye"], ["LEye", "LEar"]]
net = cv.dnn.readNetFromCaffe("pose/coco/deploy_coco.prototxt",
                              "pose/coco/pose_iter_440000.caffemodel")


def pose_estimation(frame, inWidth=380, inHeight=380):

    frame_height, frame_width = frame.shape[0], frame.shape[1]
    inp = cv.dnn.blobFromImage(frame, 1.0 / 255, (inWidth, inHeight),
                               (0, 0, 0), swapRB=False, crop=False)
    net.setInput(inp)
    start_t = time.time()
    out = net.forward()
    points = []
    for i in range(len(BODY_PARTS)):
        heatMap = out[0, i, :, :]
        _, conf, _, point = cv.minMaxLoc(heatMap)
        x = (frame_width * point[0]) / out.shape[3]
        y = (frame_height * point[1]) / out.shape[2]
        points.append((int(x), int(y)) if conf > THRESHOLD else None)

    return points


def draw_skeleton(frame, points):
    for pair in POSE_PAIRS:
        partFrom = pair[0]
        partTo = pair[1]

        id_from = BODY_PARTS[partFrom]
        id_to = BODY_PARTS[partTo]
        if points[id_from] and points[id_to]:
            cv.line(frame, points[id_from], points[id_to], (255, 74, 0), 3)
            cv.ellipse(frame, points[id_from], (4, 4), 0,
                       0, 360, (255, 255, 255), cv.FILLED)
            cv.ellipse(frame, points[id_to], (4, 4), 0, 0,
                       360, (255, 255, 255), cv.FILLED)
    return frame


def main(display=True):
    # cap = cv.VideoCapture(0)
    cap = cv.VideoCapture(
        "/Users/abhinav/Desktop/MaybeKinectGame/Pose_Estimation/exercise_videos/hips.mp4")
    ED = ExerciseDetection()
    host = ""
    port = 6969
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.bind((host, port))
    print("socket binded to port", port)
    s.listen(5)
    print("socket is listening")

    while(True):
        c, addr = s.accept()

        ret, frame = cap.read()
        height, width = frame.shape[0], frame.shape[1]
        points = pose_estimation(frame, 150, 150)
        dictified_points = convert_points_dict(points)
        ED.add_point_hip_collection(dictified_points)

        hip_movement = ED.detect_hip_movement(height, width)
        logger.info(hip_movement)
        hip_movement = hip_movement.encode()
        c.send(hip_movement)

        if(len(points)):
            pose = draw_skeleton(frame, points)
        else:
            pose = frame
        if(display):
            cv.imshow('frame', pose)
        if cv.waitKey(1) & 0xFF == ord('q'):
            break


def convert_points_dict(points):
    return {
        BODY_PARTS_INVERSE[i]: points[i] for i in range(len(points))
    }


class ExerciseDetection:
    def __init__(self, history_len=25):
        self.point_collection_history = []
        self.history_len = history_len
        self.point_hip_collection_history = []

    def add_point_collection(self, point_collection):
        if(len(self.point_collection_history) == self.history_len):
            self.point_collection_history.pop(0)
        self.point_collection_history.append(point_collection)

    def add_point_hip_collection(self, point_hip_collection):
        if(len(self.point_hip_collection_history) == self.history_len):
            self.point_hip_collection_history.pop(0)
        self.point_hip_collection_history.append(point_hip_collection)

    def validate(self, configurator):
        if len(self.point_collection_history) != self.history_len:
            return False

        first = self.point_collection_history[0]
        last = self.point_collection_history[-1]
        result = True
        for body_joint in configurator:
            fb_pos_x, fb_pos_y = first[body_joint]
            lb_pos_x, lb_pos_y = last[body_joint]
            # final greater than starting x
            if body_joint[0] == 1:
                result = result and (lb_pos_x - fb_pos_x) >= 0
            # final not greater than starting x
            else:
                result = result and (lb_pos_x - fb_pos_x) <= 0
            # final greater than starting y
            if body_joint[1] == 1:
                result = result and (lb_pos_y - fb_pos_y) >= 0
            # final not greater than starting y
            else:
                result = result and (lb_pos_y - fb_pos_y) <= 0
        return result

    def detect_basket_ball_shoot(self):
        config = {
            "LElbow": (1, -1), "RElbow": (1, -1), "LWrist": (1, -1), "RWrist": (1, -1)
        }
        result = self.validate(config)
        return result

    def detect_lounge_right(self):
        config = {
            "RKnee": (-1, 1), "LKnee": (1, 1), "RAnkle": (-1, -1), "LAnkle": (1, -1)
        }
        result = self.validate(config)
        return result

    def detect_lounge_left(self):
        config = {
            "RKnee": (1, 1), "LKnee": (-1, 1), "RAnkle": (1, -1), "LAnkle": (-1, -1)
        }
        result = self.validate(config)
        return result

    def detect_tennis_serve(self):
        config = {
            "LElbow": (1, -1), "RElbow": (1, -1), "LWrist": (1, -1), "RWrist": (1, -1)
        }
        result = self.validate(config)
        return result

    def detect_leg_lift(self):
        config = {
            "RKnee": (1, 1), "RAnkle": (1, 1), "LAnkle": (-1, 1), "RKnee": (-1, 1)
        }

    def detect_hip_movement(self, height, width):
        try:
            x_dif = self.point_hip_collection_history[-1]["LHip"][0] - \
                self.point_hip_collection_history[-6]["LHip"][0]
            y_dif = self.point_hip_collection_history[-1]["LHip"][1] - \
                self.point_hip_collection_history[-6]["LHip"][1]
            result = "none"
            dif_threshold = 0.0001
            if abs(x_dif) > abs(y_dif):
                if x_dif > dif_threshold*width:
                    result = "right"
                elif x_dif < -dif_threshold*width:
                    result = "left"
            else:
                if y_dif > dif_threshold*height:
                    result = "down"
                elif y_dif < -dif_threshold*height:
                    result = "up"
            return result
        except:
            return "none"


# img = cv.imread("./test_images/sample.jpg")
# results = pose_estimation(img)
# logger.info(convert_points_dict(results))
# logger.info(len(BODY_PARTS_INVERSE)==len(results))
# cv.imshow("result",draw_skeleton(img,results))


# cv.waitKey(0)
# cv.destroyAllWindows()

# app = Flask(__name__)

# @app.route('/',methods=['GET'])
# def home_route():
#     return "HOME ROUTE"


# @app.route('/poseestimation',methods=[''])
# def api_poseestimation():
if __name__ == "__main__":
    main(display=False)
