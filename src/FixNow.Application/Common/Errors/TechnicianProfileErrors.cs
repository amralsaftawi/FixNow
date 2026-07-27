public static class TechnicianProfileErrors
{
    

     public static readonly Error AlreadyExists =
        Error.Conflict(
            code: "TechnicianProfile.AlreadyExists",
            description: "The Technician Profile is Already Exists.");

            


            
     public static readonly Error  NotFound=
        Error.NotFound(
            code: "TechnicianProfile.NotFound",
            description: "The Technician Profile is Not Found.");


}