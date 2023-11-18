namespace Seminar_1.Models.Entities
{
    public class User
    {

        public User()
        {
            UserName = string.Empty;
            Password = string.Empty;
            SurName = string.Empty;
            Name = string.Empty;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string SurName { get; set; }
        public string Name { get; set; }

        public DateTime? LastLogin { get; set; }

        public static List<User> GetAll()
        {
            var users = new List<User>();

            users.Add(new User() { Id = 1, UserName = "Eliza", Password = "100" });
            users.Add(new User() { Id = 2, UserName = "Mihai", Password = "200" });
            users.Add(new User() { Id = 3, UserName = "Ioana", Password = "300" });
            

            return users;
        }

    }
}

