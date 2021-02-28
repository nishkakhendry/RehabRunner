# RehabRunner

RehabRunner is a hack designed to help people who are suffering from physical injuries and require rehabillitation in current times.

1. RehabRunner enables people to save a lot of money required in physical rehabillitation by providing computer vision technologies to tackle this problem.
2. It saves time by reducing the time people take to travel to the hospital.
3. Studies show that during COVID times most of the spaces inside major hospital's orthopaedic units have been repurposed to house people suffering from COVID symptoms. RehabRunner helps with social distancing and takes care of the problem of space constraints inside hospitals.
4. RehabRunner helps target children by providing a gamified way to rehabillitate children.

## Components of Hack

1. Game
2. Pose estimation
3. Exercise detection
4. Dashboard

## 1. Game

### Use

The Game has been made in order to provide a fun and interactive method to enable people to undergo what is usually a very painful and long process.

### Tech involved

1. The Game is made using unity, the game was custom built during the hackathon and is a pretty fun game.
2. The game communicates with a python backend to do most of the image processing, this connection is made using a TCP connection. Real time results are computed and sent back with minimal lag.
3. The player is then able to control his in game character to play the game.

## 2. Pose Estimation

### Use

We use pose estimation in order to detect the movement of key joints of a person, these results are useful in calculating whether a person succesfully exercises.

### Tech involved

1. The pose estimation model uses pretrained COCO model and is built in python
2. The model receives the information data from the game frontend using a TCP socket connection
3. The model then runs the predictions of each frame in a queue sending in the results, the threading enables us to get results with a delay of around 1.2 ms

## 3. Exercise Detection

### Use

We use this part to detect whether the person is performing a correct action given a sequence of points from the pose estimation

### Tech involved

1. Finding datasets for this very niche problem was very hard, we found a lot of videos from the internet of some basic exercises and recorded some videos as well.
2. We use a delta based approximation algorithm to find whether a person correctly completes an exercise, we measure how well he does it using a threshold.

## 4. Dashboard

### Use

1. The dashboard is a website we built inorder for users to measure their physical therapy progress over time based on how well they've played our game.
2. The dashboard shows calendar progress of a user
3. The dashboard shows his average score he gets in his exercise
4. The dashboard also show the time to the next doctor's consultation

### Tech involved

1. We use a mongodb server, react frontend and flask backend to run this dashboard.

## Setup guide

### Pose estimation and exercise Detection

Run the following commands on a linux based system

```bash
cd Pose_Estimation/
sh ./get_model.sh
virtualenv env
source env/bin/activate
pip install -r requirements.txt
```

To run the python program on a loop

```bash
python run_pose.py
```

### Unity

Copy paste all the folder except `Pose_Estimation`, `Dashboard` to the unity folder and run the unity game. Upon clicking run the game is built

### Dashboard

1. backend
   To install the backend for the dashboard run the following commands on a linux terminal

```bash
cd Dashboard/backend/
virtualenv env
source env/bin/activate
pip install -r requirements.txt
```

To run the backend

```bash
python main.py

```

2. frontend

To setup the frontend

```bash
cd Dashboard/frontend/
npm install
```

To run the frontend

```bash
python main.py
```
