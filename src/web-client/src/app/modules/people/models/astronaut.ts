import { Person } from './person';

export class AstronautDuty {
  id?: number;
  dutyTitle: string = '';
  rank: string = '';
  dutyStartDate?: Date;
  dutyEndDate?: Date;
}

export class Astronaut extends Person {
  rank: string = '';
  currentDutyTitle: string = '';
  careerStartDate?: Date;
  careerEndDate?: Date;
  duties: AstronautDuty[] = [];
}
