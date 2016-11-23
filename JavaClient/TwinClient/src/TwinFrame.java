import java.awt.FlowLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.List;
import java.util.concurrent.CopyOnWriteArrayList;
import java.util.concurrent.ExecutionException;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JTextArea;
import javax.swing.JTextField;
import javax.swing.Timer;

import tasks.MessageTask;
import tasks.RegisterTask;
import tasks.TwinTask;

public class TwinFrame extends JFrame {

	private static final long serialVersionUID = 8866750207262130668L;

	private List<TwinTask> futureTasks = new CopyOnWriteArrayList<>();

	public TwinFrame() {
		setTitle("Twin Client");
		setSize(400, 300);
		setLocationRelativeTo(null);
		setDefaultCloseOperation(EXIT_ON_CLOSE);
		this.setLayout(new FlowLayout());

		JTextField textField = new JTextField(20);
		this.add(textField);
		JButton button = new JButton("Register");
		this.add(button);
		
		JButton readMessagesButton = new JButton("Read Messages");
		this.add(readMessagesButton);

		readMessagesButton.addActionListener(new ActionListener() {
			
			@Override
			public void actionPerformed(ActionEvent e) {
				futureTasks.add(new MessageTask(textField.getText()));
				
			}
		});
		
		JTextArea textArea = new JTextArea(30, 30);
		textArea.setEditable(false);
		this.add(textArea);

		button.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent e) {

				futureTasks.add(new RegisterTask(textField.getText()));

			}
		});
		Timer timer = new Timer(1000 / 30, new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent e) {
				for (TwinTask task : futureTasks) {
					if (task.isDone()) {
						try {
							List<String> result = task.get();
							for (String v : result) {
								textArea.append(v + "\n");
							}
							futureTasks.remove(task);

						} catch (InterruptedException e1) {
							e1.printStackTrace();
						} catch (ExecutionException e1) {
							e1.printStackTrace();
						} catch (Exception e1) {
							e1.printStackTrace();
						}
					}
				}
			}
		});
		timer.start();
	}

}
