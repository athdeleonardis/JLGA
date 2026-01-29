namespace JLGA.Architecture.VisualNovel.Listener
{
    /// <summary>
    /// (Interface) Visual Novel Actor. Represents an actor / character that a visual novel modifies.
    /// </summary>
    public interface IVNActor : IVNListener
    {
        /// <summary>
        /// Initialize the visual novel actor.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Interpolate the position of the visual novel actor, with the first three arguments being positional arguments, and the fourth being the temporal argument.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="t"></param>
        void SetPosition(float x, float y, float z, float t);

        /// <summary>
        /// Interpolate the rotation of the visual novel actor, with the first three arguments being rotational arguments, and the fourth being the temporal argument.
        /// </summary>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        /// <param name="rz"></param>
        /// <param name="t"></param>
        void SetRotation(float rx, float ry, float rz, float t);

        /// <summary>
        /// Interpolate the scale of the visual novel actor, with the first three arguments being scale arguments, and the fourth being the temporal argument.
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="sz"></param>
        /// <param name="t"></param>
        void SetScale(float sx, float sy, float sz, float t);

        /// <summary>
        /// Set the visual appearance of the visual novel actor, identifiable by the input argument. Returns whether it was successful.
        /// </summary>
        /// <param name="appearance">The </param>
        /// <returns>True if the appearance was found. False otherwise.</returns>
        bool SetAppearance(string appearance);

        /// <summary>
        /// Plays an animation with the visual novel actor, identifiable by the input argument. Returns whether the animation was found.
        /// </summary>
        /// <param name="animation"></param>
        /// <returns>True if the animation was found. False otherwise.</returns>
        bool PlayAnimation(string animation);
    }
}
