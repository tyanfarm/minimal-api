using SimpleMinimalAPI.Data;

namespace SimpleMinimalAPI.Modules
{
    public static class StudentModule
    {
        public static WebApplication MapStudentApi(this WebApplication app)
        {
            var studentApi = app.MapGroup("/api/student");

            studentApi.MapGet("/list", () =>
            {
                return StudentCollection.Students;
            });

            return app;
        }
    }
}
