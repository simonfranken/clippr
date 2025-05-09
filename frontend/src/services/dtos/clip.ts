interface Clip {
  id: number;
  type: ClipType;
  base64Data: string;
  createdAt: string;
}

enum ClipType {
  Text = 'Text',
}

export { type Clip, ClipType };
