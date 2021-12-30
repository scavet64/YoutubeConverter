import React, { Component } from "react";

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = {
      mediaElements: [],
      loading: true,
      downloadUrl: "",
      hasSrc: false,
      currentPage: 0,
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleChange(event) {
    console.log(event);
    this.setState({ downloadUrl: event.target.value });
  }

  handleSubmit(event) {
    event.preventDefault();
    this.downloadVideo(this.state.downloadUrl);
  }

  componentDidMount() {
  }

  render() {

    return (
      <div>
        <div className="center">
          <h1 id="tabelLabel">Youtube Downloader</h1>
        </div>
        <div className="center">
          <p>Enter the URL that you want to download.</p>
        </div>

        <div className="center margin-10">
          <form onSubmit={this.handleSubmit}>
            <label>
              URL:
              <input
                className="new-url-input"
                type="text"
                name="url"
                value={this.state.downloadUrl}
                onChange={this.handleChange}
              />
            </label>
            <input type="submit" value="Download" />
          </form>
        </div>

        <div className="center">
          <video
            className={this.state.hasSrc ? "video" : "hidden"}
            id="videoId"
            controls="true"
            src={this.state.videoData}
          ></video>
        </div>
      </div>
    );
  }

  async downloadVideo(downloadUrl) {
    this.setState({ hasSrc: false, loading: true });

    var video = document.getElementById("videoId");
    
    video.pause();
    video.src = `download/vid?url=${downloadUrl}`;
    video.load();
    video.play();

    this.setState({ hasSrc: true, loading: false });
  }
}
