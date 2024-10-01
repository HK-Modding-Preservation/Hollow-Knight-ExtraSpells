using HutongGames.PlayMaker.Actions;
using System.Collections;
using Vasi;

namespace ExtraSpells
{
    public static class Helper
    {
        public static tk2dSpriteAnimator GetKnightAnimator()
        {
            return HeroController.instance.GetComponent<tk2dSpriteAnimator>();
        }

        public static void StopKnightControl()
        {
            HeroController.instance.RelinquishControl();
            HeroController.instance.StopAnimationControl();
        }

        public static void StartKnightControl()
        {
            HeroController.instance.RegainControl();
            HeroController.instance.StartAnimationControl();
        }

        public static FsmOwnerDefault GetKnightOwnerDefault()
        {
            PlayMakerFSM spellcontrol = HeroController.instance.spellControl;
            if (spellcontrol == null) { return null; }
            Tk2dPlayAnimationWithEvents action = spellcontrol.GetAction<Tk2dPlayAnimationWithEvents>("Fireball Antic",0);
            if (action == null) { return null; }
            return action.gameObject;
        }

        public static IEnumerator SendEventAfterAnim(string eventName, string anim, PlayMakerFSM fsm)
        {
            tk2dSpriteAnimator animator = GetKnightAnimator();
            
            yield return animator.PlayAnimWait(anim);

            fsm.SendEvent(eventName);
        }

        public static IEnumerator SendEventAfterTime(string eventName, float time, PlayMakerFSM fsm)
        {
            yield return new WaitForSeconds(time);

            fsm.SendEvent(eventName);
        }

        public static void PlayAnim(string anim)
        {
            tk2dSpriteAnimator animator = GetKnightAnimator();

            animator.Play(anim);
        }
    }
}
