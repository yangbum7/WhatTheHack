﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace WhatTheHack.Jobs
{
    class JobDriver_ControlMechanoid_Goto : JobDriver
    {
        public override bool TryMakePreToilReservations()
        {
            return true;
        }
        private Pawn Mech
        {
            get
            {
                return pawn.RemoteControlLink();
            }
        }
        protected override IEnumerable<Toil> MakeNewToils()
        {
            Toil gotoCell = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
            gotoCell.FailOn(() => pawn.UnableToControl() || this.Mech.DestroyedOrNull() || this.Mech.Downed || Utilities.QuickDistanceSquared(pawn.Position, Mech.Position) < 20 * 20);
            yield return gotoCell;
        }
    }
}
