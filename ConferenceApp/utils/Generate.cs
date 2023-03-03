using System;
using ConferenceApp.model.entity;

namespace ConferenceApp.utils;

public abstract class Generate
{
    public static Conference conference(int? id = null)
    {
        return new Conference
        {
            Id = id,
            Name = "conf",
            Description = "desc",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(5)
        };
    }
    
    
}