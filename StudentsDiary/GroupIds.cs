using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StudentsDiary
{
    public static class GroupIds
    {
        public static readonly string AllStudents = "Wszyscy";
        public static readonly string A1 = "A1";
        public static readonly string A2 = "A2";
        public static readonly string A3 = "A3";
        public static readonly string B1 = "B1";
        public static readonly string B2 = "B2";
        public static readonly string B3 = "B3";
        public static readonly string C1 = "C1";
        public static readonly string C2 = "C2";
        public static readonly string C3 = "C3";

        /// <summary>
        /// Gives values of all group Ids 
        /// </summary>
        /// <param name="includeAllStudents">Determines whether include AllStudents value or not.</param>
        /// <returns>List of group Ids</returns>
        public static List<string> GetListOfGroupIds(bool includeAllStudents)
        {

            var list = typeof(GroupIds).GetFields().Select(x => x.GetValue(typeof(GroupIds))).ToList();
            var listToReturn = new List<string>();
            foreach (var groupId in list)
            {
                listToReturn.Add(groupId.ToString());
            }
            if (!includeAllStudents)
            {
                try
                {
                    listToReturn.Remove(AllStudents);
                }
                catch (Exception)
                {
                    return listToReturn;
                }
            }
            return listToReturn;
        }
    }
}
