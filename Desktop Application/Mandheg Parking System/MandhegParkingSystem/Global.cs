using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandhegParkingSystem
{

    public enum FormState
    {
        Read = 0,
        Insert = 1,
        Update = 2
    }
    public class storedEmployee
    {
        public static int id;
        public static string name;
    }

    public class storedMember
    {
        public static int id;
        public static int membership_id;
        public static string name;
        public static string email;
        public static string phone_number;
        public static string address;
        public static DateTime dateofbirth;
        public static string gender;
        public static DateTime created_at;
        public static DateTime updated_at;
        public static DateTime deleted_at;
    }

    public class storedVehicles
    {
        public static int id;
    }
}
