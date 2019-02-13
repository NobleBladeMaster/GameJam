using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SoulWarriors
{
    public struct Frame
    {
        public readonly Rectangle SourceRectangle;
        public readonly Vector2 Origin;
        public readonly int FrameTime;

        /// <summary>
        /// Creates a new frame with a source rectangle, frame time and default center origin
        /// </summary>
        /// <param name="sourceRectangle">Position of frame in texture</param>
        /// <param name="frameTime">Time between this and next frame in milliseconds</param>
        public Frame(Rectangle sourceRectangle, int frameTime)
        {
            this.SourceRectangle = sourceRectangle;
            this.Origin = new Vector2(sourceRectangle.Width / 2f, sourceRectangle.Height / 2f); 
            this.FrameTime = frameTime;
        }

        /// <summary>
        /// Creates a new frame with a source rectangle, frame time and default center origin
        /// </summary>
        /// <param name="sourceRectangle">Position of frame in texture</param>
        /// <param name="origin">origin for this frame</param>
        /// <param name="frameTime">Time between this and next frame in milliseconds</param>
        public Frame(Rectangle sourceRectangle, Vector2 origin, int frameTime)
        {
            this.SourceRectangle = sourceRectangle;
            this.Origin = origin;
            this.FrameTime = frameTime;
        }
    }
    // When craeting animations it goes in order of AnimationStates + AnimationDirections
    public enum AnimationStates
    {
        Idle, Walk, Action1, Action2, Action3, Action4
    }

    public enum AnimationDirections
    {
        Up, Down, Left, Right
    }

    /// <summary>
    /// Used for animation through a list of frames
    /// </summary>
    public class Animation
    {
        private readonly List<Frame> _frames;
        private int _timeForCurrentFrame;

        public string AnimationName { get; }

        public int CurrentFrame { get; private set; }

        /// <summary>
        /// Total time for a single loop in milliseconds
        /// </summary>
        public int TotalFrameTime
        {
            get
            {
                int totalFrameTime = 0;

                for (int frame = 0; frame < _frames.Count; frame++)
                {
                    totalFrameTime += _frames[frame].FrameTime;
                }

                return totalFrameTime;
            }
        }


        /// <summary>
        /// Initializes a new animation with a name and a list of frames
        /// </summary>
        /// <param name="animationName">Identifier</param>
        /// <param name="frames">List of frames to create an animation with</param>
        public Animation(string animationName, List<Frame> frames)
        {
            AnimationName = animationName;
            this._frames = frames;
        }
        /// <summary>
        /// Initializes a new animation with an empty name and a list of frames
        /// </summary>
        /// <param name="frames">List of frames to create an animation with</param>
        public Animation(List<Frame> frames)
        {
            this._frames = frames;
            AnimationName = "";
        }

        /// <summary>
        /// Animates through the list of frames
        /// </summary>
        /// <param name="sourceRectangle">source rectangle to apply animation to</param>
        /// <param name="gameTime">Time since last update</param>
        public void Animate(ref Rectangle sourceRectangle, GameTime gameTime)
        {
            // Update time elapsed for this frame
            _timeForCurrentFrame += gameTime.ElapsedGameTime.Milliseconds;
            // If time has passed longer for this frame than this frame´s frameTime
            if (_timeForCurrentFrame >= _frames[CurrentFrame].FrameTime)
            {
                // Go to next frame in frames
                CurrentFrame = (CurrentFrame + 1) % _frames.Count;
                // Set sourceRectangle to this frame
                sourceRectangle = _frames[CurrentFrame].SourceRectangle;
                // Reset time elapsed
                _timeForCurrentFrame = 0;
            }
        }

        /// <summary>
        /// Animates through the list of frames
        /// </summary>
        /// <param name="sourceRectangle">source rectangle to apply animation to</param>
        /// <param name="origin">origin to apply animation to</param>
        /// <param name="gameTime">Time since last update</param>
        public void Animate(ref Rectangle sourceRectangle, ref Vector2 origin, GameTime gameTime)
        {
            // Update time elapsed for this frame
            _timeForCurrentFrame += gameTime.ElapsedGameTime.Milliseconds;
            // If time has passed longer for this frame than this frame´s frameTime
            if (_timeForCurrentFrame >= _frames[CurrentFrame].FrameTime)
            {
                // Go to next frame in frames
                CurrentFrame = (CurrentFrame + 1) % _frames.Count;
                // Set object´s source rectangle to this frame´s source rectangle
                sourceRectangle = _frames[CurrentFrame].SourceRectangle;
                // Set object´s origin this frame´s origin
                origin = _frames[CurrentFrame].Origin;
                // Reset time elapsed
                _timeForCurrentFrame = 0;
            }
        }

        /// <summary>
        /// Sets animation to the specified frame
        /// </summary>
        /// <param name="sourceRectangle"></param>
        /// <param name="frameToSet"></param>
        public void SetToFrame(ref Rectangle sourceRectangle, int frameToSet)
        {
            // Set animation to first frame
            CurrentFrame = frameToSet % _frames.Count;
            // Set sourceRectangle to the first frame
            sourceRectangle = _frames[CurrentFrame].SourceRectangle;
            // Reset time elapsed
            _timeForCurrentFrame = 0;
        }

        /// <summary>
        /// Resets animation to the 0:th frame
        /// </summary>
        public void Reset()
        {
            // Set animation to  frame
            CurrentFrame = 0;
            // Reset time elapsed
            _timeForCurrentFrame = 0;

        }
    }

    /// <summary>
    /// Handles a set of animations and character animation with them
    /// </summary>
    public class AnimationSet
    {
        private readonly List<Animation> Animations;

        public AnimationDirections AnimationDirection;
        public AnimationStates AnimationState;

        /// <summary>
        /// Returns the animation dependent on state and direction
        /// </summary>
        public Animation CurrentAnimation
        {
            get
            {
                string identifier = (AnimationState.ToString() + AnimationDirection.ToString());
                foreach (Animation animation in Animations)
                {
                    if (animation.AnimationName.Equals(identifier))
                    {
                        return animation;
                    }
                }

                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Instantiates a new AnimationSet with a list of animations and initial state and direction
        /// </summary>
        /// <param name="animations"></param>
        /// <param name="animationState"></param>
        /// <param name="initialDirection"></param>
        public AnimationSet(List<Animation> animations, AnimationStates animationState, AnimationDirections initialDirection)
        {
            Animations = animations;
            AnimationState = animationState;
            AnimationDirection = initialDirection;
        }

        /// <summary>
        /// Animate the current animation
        /// </summary>
        /// <param name="sourceRectangle"></param>
        /// <param name="origin"></param>
        /// <param name="gameTime">Time since last update</param>
        public void UpdateAnimation(ref Rectangle sourceRectangle, ref Vector2 origin, GameTime gameTime)
        {
            // Animate CurrentAnimation
            CurrentAnimation.Animate(ref sourceRectangle, ref origin, gameTime);

            // Reset all other animations except from the CurrentAnimation
            foreach (Animation animation in Animations)
            {
                if (ReferenceEquals(animation, CurrentAnimation)) { return; }
                animation.Reset();
            }
        }
    }
}
