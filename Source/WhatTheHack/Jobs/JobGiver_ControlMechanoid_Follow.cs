﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace WhatTheHack.Jobs
{
    class JobGiver_ControlMechanoid_Follow : ThinkNode_JobGiver
    {


        private int FollowJobExpireInterval
        {
            get
            {
                return 200;
            }
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            Pawn followee = pawn.RemoteControlLink();
            if (followee == null)
            {
                Log.Warning(base.GetType() + "has null followee.");
                return null;
            }
            if (!GenAI.CanInteractPawn(pawn, followee))
            {
                return null;
            }
            float radius = 30f;//TODO: no magic number
            if ((!followee.pather.Moving || (float)followee.pather.Destination.Cell.DistanceToSquared(pawn.Position) <= radius * radius) && (float)followee.Position.DistanceToSquared(pawn.Position) <= radius * radius)
            {
                return null;
            }
            IntVec3 root;
            if (followee.pather.Moving && followee.pather.curPath != null)
            {
                root = followee.pather.curPath.FinalWalkableNonDoorCell(followee.Map);
            }
            else
            {
                root = followee.Position;
            }
            Job job = new Job(WTH_DefOf.ControlMechanoid_Goto, followee.Position);
            job.expiryInterval = this.FollowJobExpireInterval;
            job.checkOverrideOnExpire = true;
            if (pawn.mindState.duty != null && pawn.mindState.duty.locomotion != LocomotionUrgency.None)
            {
                job.locomotionUrgency = pawn.mindState.duty.locomotion;
            }
            return job;
        }
    }
}
