import { VideoMetadata } from "./video-metadata";

export interface AudioDownloadHistory {
  id: number;
  videoId: string;
  bitrateInKilobytesPerSecond: number;
  createdOn: Date;
  videoMetadata: VideoMetadata;
}
