﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Core;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace UnitTests
{
    [TestFixture()]
    class RoomManagerTester
    {
        private RoomManager m_CurretManager = RoomManager.GetInstance();
        readonly string m_TestId = Guid.NewGuid().ToString();

       [Test]
        public void TestForRoomsSaveFile()
        {
            Assert.IsTrue(File.Exists(SerializationManager.RoomSaveFile), "File not created.");
        }

        [Test]
        public void AddNewRoom()
        {
            string testName = "Test room";
            string testDescription = "Test description";
            string testType = "Test type";
            int testFloorNumber = 10;
            int testRoomNumber = 102;

            m_CurretManager.CreateRoom(testName, testDescription, testType, testFloorNumber, testRoomNumber, m_TestId);

            Assert.IsTrue(m_CurretManager.Rooms.ContainsKey(m_TestId));
            Assert.IsTrue(m_CurretManager.Rooms[m_TestId].Name==testName);
            Assert.IsTrue(m_CurretManager.Rooms[m_TestId].Description == testDescription);
            Assert.IsTrue(m_CurretManager.Rooms[m_TestId].Type == testType);
            Assert.IsTrue(m_CurretManager.Rooms[m_TestId].Floor==testFloorNumber);
            Assert.IsTrue(m_CurretManager.Rooms[m_TestId].RoomNumber==testRoomNumber);
            Assert.IsTrue(m_CurretManager.Rooms[m_TestId].Id==m_TestId);

            Dictionary<string, Room> testDictionary = SerializationManager.GetRooms();

            Assert.AreEqual(1, testDictionary.Count);
            Assert.IsTrue(testDictionary.ContainsKey(m_TestId));
            Assert.IsTrue(testDictionary[m_TestId].Name == testName);
            Assert.IsTrue(testDictionary[m_TestId].Description == testDescription);
            Assert.IsTrue(testDictionary[m_TestId].Type == testType);
            Assert.IsTrue(testDictionary[m_TestId].Floor == testFloorNumber);
            Assert.IsTrue(testDictionary[m_TestId].RoomNumber == testRoomNumber);
            Assert.IsTrue(testDictionary[m_TestId].Id == m_TestId);
        }

        [Test]
        public void TestGetRooms()
        {
            Dictionary<string, string > currentRooms = m_CurretManager.GetRooms();
            Assert.IsTrue(currentRooms[m_TestId] == "Test roomFloor Number: 10Room Number: 102");
        }

        //[Test]
        public void RemoveRoom()
        {
            m_CurretManager.DeleteRoom(m_TestId);

            Assert.AreEqual(0, m_CurretManager.Rooms.Count);
            Dictionary<string, Room> testDictionary = SerializationManager.GetRooms();
            Assert.AreEqual(0, testDictionary.Count);
        }

       
    }
}