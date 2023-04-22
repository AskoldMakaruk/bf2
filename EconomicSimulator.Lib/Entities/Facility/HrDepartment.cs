namespace EconomicSimulator.Lib.Entities;

public record OnboardingWorker(Worker Worker, JobType JobType);

public class HrDepartment
{
    private readonly IFacility Facility;
    private List<JobPost> JobPosts { get; } = new();
    public Queue<OnboardingWorker> OnboardingQueue { get; } = new();

    public HrDepartment(IFacility facility)
    {
        Facility = facility;
    }

    public void AddPost(JobType type)
    {
        JobPosts.Add(new JobPost(Facility, type, type.MaxWorkers - Facility.GetFreeWorkers().Count()));
    }

    public IEnumerable<JobPost> GetJobPosts()
    {
        return JobPosts.Where(a => a.Slots > 0);
    }

    public bool TryHire(Worker worker, JobPost post)
    {
        if (JobPosts.FirstOrDefault(a => a.Guid == post.Guid) is not { } jobPost)
        {
            return false;
        }

        if (jobPost.Slots < 1)
        {
            JobPosts.Remove(post);
            return false;
        }

        jobPost.Slots--;
        Facility.Workers.Add(worker);
        return true;
    }
}