import { Teacher } from "src/app/models/teacher";


export const DBTEACHERSDATA: Teacher[]=[
  {
    id: "teacher1", firstName: "Dennis", lastName: "Cools", emailAddress: "d.cools@fontys.nl"
  },
  {
    id: "teacher2", firstName: "Haran", lastName: "Moraal", emailAddress: "h.moraal@fontys.nl", groups: [
      {id: "joafs2", name: "M02", period: "S02", startDate: new Date("2019-01-16"), endDate: new Date("2019-07-16")}]

  }
]
