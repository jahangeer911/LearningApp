import { Photos } from "./photos";

export interface Users {
    id: number;
  username: string;
  knownAs: string;
  age: number;
  created: Date;
  lastActive: Date;
  photoUrl: string;
  city: string;
  country: string;
  interests?: string;
  introduction?: string;
  lookingfor?: string;
  photos?: Photos[];
}
