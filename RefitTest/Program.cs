using System;
using Refit;
using RefitTest;

var client = RestService.For<ITimeslots>("https://localhost:44397/api/");

var templates = await client.GetTimeSlotTemplates(1075);
Console.WriteLine(string.Join("\n", templates));
