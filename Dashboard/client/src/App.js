import React, { useState, useEffect } from "react";
import { Header } from "./components";
import { Line } from "react-chartjs-2";

const App = () => {
  const [userInfo, setUserInfo] = useState({});
  const [lineData, setLineData] = useState({});
  useEffect(() => {
    (async () => {
      let res = await fetch(
        "https://bf3ee0ba9479.ngrok.io/user/603abca241f00c02fe4d9490"
      );
      let data = await res.json();
      let lineData = [];
      for (let keys in data.currweek) {
        lineData.push(data.currweek[keys]);
      }
      let graphData = {
        labels: Object.keys(data.currweek).sort(),
        datasets: [
          {
            label: "Weekly Progress",
            fill: false,
            lineTension: 0.1,
            backgroundColor: "rgba(75,192,192,0.4)",
            borderColor: "rgba(75,192,192,1)",
            borderCapStyle: "butt",
            borderDash: [],
            borderDashOffset: 0.0,
            borderJoinStyle: "miter",
            pointBorderColor: "rgba(75,192,192,1)",
            pointBackgroundColor: "#fff",
            pointBorderWidth: 1,
            pointHoverRadius: 5,
            pointHoverBackgroundColor: "rgba(75,192,192,1)",
            pointHoverBorderColor: "rgba(220,220,220,1)",
            pointHoverBorderWidth: 2,
            pointRadius: 1,
            pointHitRadius: 10,
            data: lineData,
          },
        ],
      };
      setUserInfo(data);
      setLineData(graphData);
    })();
  }, []);
  return (
    <div>
      <Header name={userInfo.name} />
      <div style={{ paddingTop: "1%" }}></div>
      <div className="row">
        <div className="col m1"></div>
        <div className="col m10">
          <div className="card white" style={{ height: "40vh" }}>
            <div style={{ padding: "2%" }}>
              <Line
                data={lineData}
                options={{ maintainAspectRatio: false }}
                height={100}
              />
              <h5 className="center">Scores For This Week</h5>
            </div>
          </div>
        </div>
        <div className="col m1"></div>
      </div>
      <div className="row">
        <div className="col m1"></div>
        <div className="col m4">
          <div className="card" style={{ height: "40vh" }}>
            <div style={{ padding: "2%" }}>
              <h5 className="center">Average Score</h5>
              <h1 className="center">{userInfo.average_score}</h1>
              <p className="center">Good Job! Keep up the good work :)</p>
            </div>
          </div>
        </div>
        <div className="row">
          <div className="col m1"></div>
          <div className="col m5">
            <div className="card" style={{ height: "40vh" }}>
              <div style={{ padding: "2%" }}>
                <h5 className="center">Upcoming Appointments</h5>
                <h1 className="center">{userInfo.doc_time}</h1>
                <h5 className="center">Weeks</h5>
                <p className="center">Regular check up with the doctor</p>
              </div>
            </div>
          </div>
          <div className="col m1"></div>
        </div>
      </div>
    </div>
  );
};

export default App;
