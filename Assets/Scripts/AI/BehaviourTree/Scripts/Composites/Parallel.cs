using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Harmony.AI {
    public class Parallel : CompositeNode
    {
        public bool abortOnFailure = true;

        List<State> childrenLeftToExecute = new List<State>();

        protected override void OnStart() {
            childrenLeftToExecute.Clear();
            children.ForEach(a => {
                childrenLeftToExecute.Add(State.Running);
            });
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            bool stillRunning = false;
            bool allFailed = true;
            for (int i = 0; i < childrenLeftToExecute.Count(); ++i) {
                if (childrenLeftToExecute[i] == State.Running) {
                    var status = children[i].Update();
                    if (abortOnFailure && status == State.Failure) {
                        AbortRunningChildren();
                        return State.Failure;
                    }

                    if (status == State.Running) {
                        stillRunning = true;
                        allFailed = false;
                    }else if (status == State.Success)
                    {
                        allFailed = false;
                    }

                    if(abortOnFailure)
                        childrenLeftToExecute[i] = status;
                }
            }

            return stillRunning ? State.Running : allFailed ? State.Failure : State.Success;
        }

        void AbortRunningChildren() {
            for (int i = 0; i < childrenLeftToExecute.Count(); ++i) {
                if (childrenLeftToExecute[i] == State.Running) {
                    children[i].Abort();
                }
            }
        }
    }
}