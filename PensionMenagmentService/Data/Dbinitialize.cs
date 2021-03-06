﻿using PensionMenagmentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PensionMenagmentService.Data
{
    public static class DBbinitialize
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Rooms.Any())
            {
                return;
            }

            var rooms = new Room[]
            {
                new Room{Number=1, nubmerbeds=(RoomType)1, ReguralPrice=200},
                new Room{Number=2, nubmerbeds=(RoomType)1, ReguralPrice=200},
                new Room{Number=3, nubmerbeds=(RoomType)1, ReguralPrice=200},
                new Room{Number=4, nubmerbeds=(RoomType)0, ReguralPrice=150},
                new Room{Number=5, nubmerbeds=(RoomType)0, ReguralPrice=150}
            };
             foreach (Room n in rooms)
            {
                context.Rooms.Add(n);
            }
            context.SaveChanges();
        }
    }
}
