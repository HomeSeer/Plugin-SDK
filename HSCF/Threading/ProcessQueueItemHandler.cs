namespace HSCF.Threading
{
    /// <summary>
    /// A delegate to used by QueueProcessorThread to raise processing event
    /// </summary>
    /// <typeparam name="T">Type of the item to process</typeparam>
    /// <param name="sender">The object which raises event</param>
    /// <param name="e">Event arguments</param>
    public delegate void ProcessQueueItemHandler<T>(object sender, ProcessQueueItemEventArgs<T> e);
}