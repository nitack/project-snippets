# Display the comments of a thread
class CommentThread(generic.ListView):
    template_name = 'forum/comment_thread.html'
    context_object_name = 'comments'
    paginated_by = 10
    ordering = ['-DateCreated']


    def get_queryset(self, *args, **kwargs):
        pk = self.kwargs['pk']
        return Comment.objects.filter(Thread_id=pk).order_by('DateCreated').reverse


    def get_context_data(self, *args, **kwargs):
        context = super(CommentThread, self).get_context_data(**kwargs)
        pk = self.kwargs['pk']
        thread = Thread.objects.get(id = pk)
        count = thread.ViewCount
        thread.ViewCount = count + 1
        thread.save()
        context['thread'] = Thread.objects.filter(id = pk).last
        context['form'] = CommentCreateForm()

        return context