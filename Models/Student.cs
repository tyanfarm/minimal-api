﻿namespace SimpleMinimalAPI.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Course { get; set; }
        public int Points { get; set; }
    }
}
