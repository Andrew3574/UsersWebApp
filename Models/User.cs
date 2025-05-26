using System;
using System.Collections.Generic;

namespace WebApp_Tak4.Models;

public partial class User
{
    public int Userid { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname{ get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public DateTime Lastlogin { get; set; } = DateTime.Now;

    public UserState UserState { get; set; } = UserState.active;
}
