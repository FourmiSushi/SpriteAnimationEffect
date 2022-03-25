using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
// ReSharper disable UnusedMember.Global

namespace FourmiSushi.Animation
{
    /// <summary>
    /// エフェクトを再生するためのコンポーネント
    /// </summary>
    public class AnimationEffectPlayer : MonoBehaviour
    {
        private AnimationEffect _skillEffectPrefab;

        protected void Awake()
        {
            _skillEffectPrefab = Resources.Load<AnimationEffect>("Prefabs/アニメーション/SkillEffect");
        }


        /// <summary>
        /// エフェクトを単発で再生する
        /// </summary>
        /// <param name="anim">再生するアニメーション</param>
        /// <param name="position">再生する座標</param>
        /// <param name="scale">再生サイズ</param>
        /// <param name="angle">再生角度</param>
        /// <returns>エフェクト終了時に終了するUniTask</returns>
        public UniTask PlayEffect(AnimationClip anim, Vector2 position, Vector2 scale, float angle = 0)
        {
            var effect = Instantiate(
                _skillEffectPrefab,
                transform
            );

            var transform1 = effect.transform;
            transform1.position = position;
            transform1.localScale = scale;
            transform1.Rotate(Vector3.forward, angle);

            var eff = effect.Play(anim);

            return eff.ContinueWith(() => Destroy(effect.gameObject));
        }

        /// <summary>
        /// エフェクトをループさせて時間制限なしで再生 自分で止めたい時用
        /// ゲームオブジェクトが返るのでエフェクトを移動させたい時も使う
        /// </summary>
        /// <param name="anim">再生するアニメーション</param>
        /// <param name="position">再生する座標</param>
        /// <param name="scale">再生サイズ</param>
        /// <param name="angle">再生角度</param>
        /// <returns>エフェクト .Stop()でエフェクトを終了できる</returns>
        public AnimationEffect PlayEffectLoop(AnimationClip anim, Vector2 position, Vector2 scale, float angle = 0)
        {
            var effect = Instantiate(
                _skillEffectPrefab,
                transform
            );

            var transform1 = effect.transform;
            transform1.position = position;
            transform1.localScale = scale;
            transform1.Rotate(Vector3.forward, angle);

            effect.Play(anim, true).ContinueWith(() => Destroy(effect.gameObject));

            return effect;
        }

        /// <summary>
        /// エフェクトをループさせて指定した時間再生
        /// </summary>
        /// <param name="anim">再生するアニメーション</param>
        /// <param name="timeSpan">再生する時間</param>
        /// <param name="position">再生する座標</param>
        /// <param name="scale">再生サイズ</param>
        /// <param name="angle">再生角度</param>
        /// <returns>指定した時間が経つと終了するUniTask</returns>
        public UniTask PlayEffectFor(AnimationClip anim, TimeSpan timeSpan, Vector2 position, Vector2 scale,
            float angle = 0)
        {
            var effect = Instantiate(
                _skillEffectPrefab,
                transform
            );

            var transform1 = effect.transform;
            transform1.position = position;
            transform1.localScale = scale;
            transform1.Rotate(Vector3.forward, angle);

            effect.Play(anim, true);

            return UniTask.Delay(timeSpan, DelayType.DeltaTime).ContinueWith(() => Destroy(effect.gameObject));
        }
    }
}