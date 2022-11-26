using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;
using API.Models;

namespace API.Models
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Bio { get; set; } = Const.ns;
        public string Interests { get; set; } = Const.ns;
        public string? Country { get; set; } = Const.ns;
        public string? Gender { get; set; } = Const.ns;
        public bool IsAdmin { get; set; } = false;
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Post> Posts { get; set; } = new List<Post>();
        public ProfilePicture? ProfilePicture { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }

        internal void Deconstruct(out int id, out string username, out string name, out string? profilePicture)
        {
            id = Id;
            username = Username;
            name = Name;
            profilePicture = ProfilePicture?.Url;
        }

        internal void Deconstruct(out string username, out string name, out DateTime? dateOfBirth, out string bio, out string? gender, out string? country)
        {
            username = Username;
            name = Name;
            dateOfBirth = DateOfBirth;
            bio = Bio;
            gender = Gender;
            country = Country;
        }
    }
}