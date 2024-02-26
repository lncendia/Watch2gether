﻿namespace PJMS.AuthService.Data.IdentityServer.Entities;

public class ClientGrantType
{
    public int Id { get; set; }
    public string GrantType { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }
}