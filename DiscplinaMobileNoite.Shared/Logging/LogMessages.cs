namespace DiscplinaMobileNoite.Shared.Logging
{
    public static class LogMessages
    {
        //User
        public static string InvalidUserInputs() => "Message: Invalid inputs to User.";
        public static string NullOrEmptyUserEmail() => "Message: The Email field is null, empty, or whitespace.";
        public static string UpdatingErrorUser(Exception ex) => $"Message: Error updating User: {ex.Message}";
        public static string UpdatingSuccessUser() => "Message: Successfully updated User.";
        public static string UserNotFound(string action) => $"Message: User not found for {action} action.";
        public static string AddingUserError(Exception ex) => $"Message: Error adding a new User: {ex.Message}";
        public static string AddingUserSuccess() => "Message: Successfully added a new User.";
        public static string GetAllUserError(Exception ex) => $"Message: Error to loading the list User: {ex.Message}";
        public static string GetAllUserSuccess() => "Message: GetAll with success User.";

        //AttendanceRecord
        public static string InvalidAttendanceRecordInputs() => "Message: Invalid inputs to Attendance Record.";
        public static string UpdatingErrorAttendanceRecord(Exception ex) => $"Message: Error updating Attendance Record: {ex.Message}";
        public static string UpdatingSuccessAttendanceRecord() => "Message: Successfully updated Attendance Record.";
        public static string AttendanceRecordNotFound(string action) => $"Message: Attendance Record not found for {action} action.";
        public static string AddingAttendanceRecordError(Exception ex) => $"Message: Error adding a new Attendance Record: {ex.Message}";
        public static string AddingAttendanceRecordSuccess() => "Message: Successfully added a new Attendance Record.";
        public static string GetAllAttendanceRecordError(Exception ex) => $"Message: Error to loading the list Attendance Record: {ex.Message}";
        public static string GetAllAttendanceRecordSuccess() => "Message: GetAll with success Attendance Record.";

        //AttendanceJustification

        public static string InvalidAttendanceJustificationInputs() => "Message: Invalid inputs to Attendance Justification.";
        public static string UpdatingErrorAttendanceJustification(Exception ex) => $"Message: Error updating Attendance Justification: {ex.Message}";
        public static string UpdatingSuccessAttendanceJustification() => "Message: Successfully updated Attendance Justification.";
        public static string AttendanceJustificationNotFound(string action) => $"Message: Attendance Justification not found for {action} action.";
        public static string AddingAttendanceJustificationError(Exception ex) => $"Message: Error adding a new Attendance Justification: {ex.Message}";
        public static string AddingAttendanceJustificationSuccess() => "Message: Successfully added a new Attendance Justification.";
        public static string GetAllAttendanceJustificationError(Exception ex) => $"Message: Error to loading the list Attendance Justification: {ex.Message}";
        public static string GetAllAttendanceJustificationSuccess() => "Message: GetAll with success Attendance Justification.";
    }
}