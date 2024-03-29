using System.Collections.Generic;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Newtonsoft.Json;

public class Seed {

    private DataContext _context;

    public Seed(DataContext context)
    {
        _context = context;
    }

    public void SeedUsers(){
        var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
        var users = JsonConvert.DeserializeObject<List<User>>(userData);
        
        foreach(var user in users){

            byte[] password, passwordSalt;
            CreatePasswordHash("password", out password, out passwordSalt);
            user.PasswordHash = password; user.PasswordSalt = passwordSalt;
            user.Username = user.Username.ToLower();
            _context.Users.Add(user);
            _context.SaveChanges();
        }

    }
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt = hmac.Key;
                passwordHash= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }
}