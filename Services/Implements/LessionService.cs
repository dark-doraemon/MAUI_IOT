using MAUI_IOT.Models;
using MAUI_IOT.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Services.Implements
{
    internal class LessionService : ILessionService
    {
        ObservableCollection<Lesson> lessons;
        public LessionService()
        {
            lessons = new ObservableCollection<Lesson>
            {
                new Lesson
                {
                    LessonId = "1",
                    LessonName = "Bai 1",
                    LessonDescription = "Thi nghiem gia toc",
                    LessonStatus = "Chua lam",
                    LessonPoint = 10,
                    LessonImage = "https://images.theconversation.com/files/191827/original/file-20171025-25516-g7rtyl.jpg?ixlib=rb-4.1.0&rect=0%2C70%2C7875%2C5667&q=45&auto=format&w=926&fit=clip"

                },

                new Lesson
                {
                    LessonId = "2",
                    LessonName = "Bai 2",
                    LessonDescription = "Thi nghiem van toc",
                    LessonStatus = "Chua lam",
                    LessonPoint = 10,
                    LessonImage = "https://images.theconversation.com/files/191827/original/file-20171025-25516-g7rtyl.jpg?ixlib=rb-4.1.0&rect=0%2C70%2C7875%2C5667&q=45&auto=format&w=926&fit=clip"

                },

                new Lesson
                {
                    LessonId = "3",
                    LessonName = "Bai 3",
                    LessonDescription = "Thi nghiem va cham",
                    LessonStatus = "Chua lam",
                    LessonPoint = 10,
                    LessonImage = "https://images.theconversation.com/files/191827/original/file-20171025-25516-g7rtyl.jpg?ixlib=rb-4.1.0&rect=0%2C70%2C7875%2C5667&q=45&auto=format&w=926&fit=clip"

                },

                new Lesson
                {
                    LessonId = "4",
                    LessonName = "Bai 4",
                    LessonDescription = "Thi nghiem nhiet do",
                    LessonStatus = "Chua lam",
                    LessonPoint = 10,
                    LessonImage = "https://images.theconversation.com/files/191827/original/file-20171025-25516-g7rtyl.jpg?ixlib=rb-4.1.0&rect=0%2C70%2C7875%2C5667&q=45&auto=format&w=926&fit=clip"
                },
            };
        }

        public Lesson GetLessonById(string id)
        {
            var lesson = lessons.FirstOrDefault(l => l.LessonId == id);
            if (lesson == null)
            {
                return null;
            }
            return lesson;
        }

        public ObservableCollection<Lesson> GetLessons()
        {
            return lessons;
        }


    }
}
