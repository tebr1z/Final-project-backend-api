using System;

namespace LmsApiApp.Application.Exceptions
{
    public class CourseNotFoundException : Exception
    {
        public CourseNotFoundException(int courseId)
            : base($"Ders Tapılmadı. dərs ID: {courseId}")
        {
        }
    }
}
