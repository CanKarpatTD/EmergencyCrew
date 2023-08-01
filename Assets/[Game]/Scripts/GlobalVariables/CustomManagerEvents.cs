using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GlobalVariables
{
    /// <summary>
    /// Add custom managers events here.
    /// <example> <code> public const string SomeEvent = nameof(SomeEvent); </code> </example>
    /// </summary>
    public static partial class CustomManagerEvents
    {
        public const string PatientWaiting = nameof(PatientWaiting);
        public const string PlayerIn = nameof(PlayerIn);
        public const string PlayerOut = nameof(PlayerOut);
        public const string PatientIn = nameof(PatientIn);
        public const string PatientComingToRoom = nameof(PatientComingToRoom);
        public const string GetMoney = nameof(GetMoney);
        public const string AddMoney = nameof(AddMoney);
        public const string SetNewPositions = nameof(SetNewPositions);
        public const string PatientInThisRoom = nameof(PatientInThisRoom);
        public const string SetRoom = nameof(SetRoom);
        public const string SetChair = nameof(SetChair);
        public const string SetRoomList = nameof(SetRoomList);
    }
}
