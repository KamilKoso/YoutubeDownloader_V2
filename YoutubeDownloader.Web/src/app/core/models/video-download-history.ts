import { VideoMetadata } from "./video-metadata";

export interface VideoDownloadHistory {
  id: number;
  videoId: string;
  bitrateInKilobytesPerSecond: Nullable<number>;
  qualityLabel: string;
  createdOn: Date;
  videoMetadata: VideoMetadata;
}
