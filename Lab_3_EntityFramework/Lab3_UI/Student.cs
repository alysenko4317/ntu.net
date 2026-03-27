
namespace Lab2_App
{
    class Student
    {
        private string name;
        private string surname;
        private string recordBookNumber;

        public Student(string name, string surname, string recordBookNumber) {
            this.name = name;
            this.surname = surname;
            this.recordBookNumber = recordBookNumber;
        }

        public string getName() {
            return this.name;
        }

        public string getSurname() {
            return this.surname;
        }

        public string getRecordBookNumber() {
            return this.recordBookNumber;
        }

        public void setName(string name) {
            this.name = name;
        }

        public void setSurname(string surname) {
            this.surname = surname;
        }

        public void setRecordBookNumber(string recordBookNumber) {
            this.recordBookNumber = recordBookNumber;
        }
    }
}
