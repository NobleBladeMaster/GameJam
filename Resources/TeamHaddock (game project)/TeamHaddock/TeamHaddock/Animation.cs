using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
// Created by Alexander 11-25
namespace TeamHaddock
{
    public struct Frame
    {
        public readonly Rectangle sourceRectangle;
        public readonly Vector2 origin;
        public readonly int frameTime;

        /// <summary>
        /// Creates a new frame with a source rectangle, frame time and default center origin
        /// </summary>
        /// <param name="sourceRectangle">Position of frame in texture</param>
        /// <param name="frameTime">Time between this and next frame in milliseconds</param>
        public Frame(Rectangle sourceRectangle, int frameTime)
        {
            this.sourceRectangle = sourceRectangle;
            this.origin = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2); 
            this.frameTime = frameTime;
        }

        /// <summary>
        /// Creates a new frame with a source rectangle, frame time and default center origin
        /// </summary>
        /// <param name="sourceRectangle">Position of frame in texture</param>
        /// <param name="origin">origin for this frame</param>
        /// <param name="frameTime">Time between this and next frame in milliseconds</param>
        public Frame(Rectangle sourceRectangle, Vector2 origin, int frameTime)
        {
            this.sourceRectangle = sourceRectangle;
            this.origin = origin;
            this.frameTime = frameTime;
        }
    }

    public class Animation
    {
        public string name;
        public List<Frame> frames;
        private int timeForCurrentFrame;

        /// <summary>
        /// Total time for a single loop in milliseconds
        /// </summary>
        public int TotalFrameTime
        {
            get
            {
                int totalFrameTime = 0;

                for (int frame = 0; frame < frames.Count; frame++)
                {
                    totalFrameTime += frames[frame].frameTime;
                }

                return totalFrameTime;
            }
        }

        public int CurrentFrame { get; private set; }

        /// <summary>
        /// Initializes a new animation with a name and list of frames
        /// </summary>
        /// <param name="name"></param>
        /// <param name="frames"></param>
        public Animation(string name, List<Frame> frames)
        {
            this.name = name;
            this.frames = frames;
        }

        public Animation(List<Frame> frames)
        {
            name = "";
            this.frames = frames;
        }

        /// <summary>
        /// Animates through the list of frames
        /// </summary>
        /// <param name="sourceRectangle">source rectangle to apply animation to</param>
        /// <param name="gameTime"></param>
        public void Animate(ref Rectangle sourceRectangle, GameTime gameTime)
        {
            // Update time elapsed for this frame
            timeForCurrentFrame += gameTime.ElapsedGameTime.Milliseconds;
            // If time has passed longer for this frame than this frame´s frameTime
            if (timeForCurrentFrame >= frames[CurrentFrame].frameTime)
            {
                // Go to next frame in frames
                CurrentFrame = (CurrentFrame + 1) % frames.Count;
                // Set sourceRectangle to this frame
                sourceRectangle = frames[CurrentFrame].sourceRectangle;
                // Reset time elapsed
                timeForCurrentFrame = 0;
            }
        }

        /// <summary>
        /// Animates through the list of frames
        /// </summary>
        /// <param name="sourceRectangle">source rectangle to apply animation to</param>
        /// <param name="origin">origin to apply animation to</param>
        /// <param name="gameTime"></param>
        public void Animate(ref Rectangle sourceRectangle, ref Vector2 origin, GameTime gameTime)
        {
            // Update time elapsed for this frame
            timeForCurrentFrame += gameTime.ElapsedGameTime.Milliseconds;
            // If time has passed longer for this frame than this frame´s frameTime
            if (timeForCurrentFrame >= frames[CurrentFrame].frameTime)
            {
                // Go to next frame in frames
                CurrentFrame = (CurrentFrame + 1) % frames.Count;
                // Set object´s source rectangle to this frame´s source rectangle
                sourceRectangle = frames[CurrentFrame].sourceRectangle;
                // Set object´s origin this frame´s origin
                origin = frames[CurrentFrame].origin;
                // Reset time elapsed
                timeForCurrentFrame = 0;
            }
        }


        public void SetToFrame(ref Rectangle sourceRectangle, int frameToSet)
        {
            // Set animation to first frame
            CurrentFrame = frameToSet % frames.Count;
            // Set sourceRectangle to the first frame
            sourceRectangle = frames[CurrentFrame].sourceRectangle;
            // Reset time elapsed
            timeForCurrentFrame = 0;
        }

        /// <summary>
        /// Resets animation
        /// </summary>
        public void Reset()
        {
            // Set animation to  frame
            CurrentFrame = 0;
            // Reset time elapsed
            timeForCurrentFrame = 0;

        }
    }
}    
