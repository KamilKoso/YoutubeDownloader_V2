import { TimeSpan } from "@app/shared/helpers/timespan";

export interface VideoMetadata {
  author: string;
  channelId: string;
  title: string;
  videoUrl: string;
  thumbnailUrl: any;
  duration: TimeSpan;
  views: number;
  bitratesKilobytesPerSecond: number[];
  videoQualityLabels: string[];
}
