package tasks;

import java.util.List;

public interface TwinTask {

	public boolean isDone();
	public List<String> get() throws Exception;
	
}
