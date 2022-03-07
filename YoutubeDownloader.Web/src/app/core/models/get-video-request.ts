export interface GetVideoRequest {
  videoUrl: string;
  videoTitle: string;
  videoAuthor: string;
  signalrConnectionId: string;
  videoQualityLabel: string;
  bitrate: Nullable<number>;
}
