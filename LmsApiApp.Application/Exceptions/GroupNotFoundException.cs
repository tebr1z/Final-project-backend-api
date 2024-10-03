using System;

namespace LmsApiApp.Application.Exceptions
{
    public class GroupNotFoundException : Exception
    {
        public GroupNotFoundException(int groupId)
            : base($"Grup Tapılmadı. Grup ID: {groupId}")
        {
        }
    }
}
