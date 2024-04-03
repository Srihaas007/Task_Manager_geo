﻿using Task_Management.Services;
using System;
using System.Threading.Tasks;

namespace Task_Management.Services
{
    public interface IAppNotificationService
    {
        Task ScheduleNotification(int id, string title, string message, DateTime notifyTime);
        Task CancelNotification(int id);
    }
}
