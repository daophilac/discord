package com.peanut.discord;

import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.view.ContextThemeWrapper;
import android.util.Log;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.peanut.discord.filedealer.InternalFileReader;
import com.peanut.discord.filedealer.InternalFileWriter;
import com.peanut.discord.resources.Data;
import com.peanut.discord.resources.Route;
import com.peanut.discord.tools.APICaller;
import com.peanut.discord.tools.JsonBuilder;
import com.peanut.discord.worker.SingleWorker;

import static com.peanut.discord.MainActivity.themeId;

public class LoginActivity extends AppCompatActivity {
//    private static final int
    public Handler handlerLogin;
    private APICaller apiCaller;
    private JsonBuilder jsonBuilder;
    private EditText editTextEmail;
    private EditText editTextPassword;
    private String email;
    private String password;
    private TextView textViewSignUp;
    private Button buttonLogin;
    private InternalFileReader internalFileReader;
    public SingleWorker singleWorker;

    @Override
    protected void onDestroy() {
        super.onDestroy();
        singleWorker.quit();
    }

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        MainActivity.themeInflater = getLayoutInflater().cloneInContext(new ContextThemeWrapper(this, themeId));
        setTheme(themeId);
        ExecutionTimeTracker tracker = new ExecutionTimeTracker();
        initializeGlobalVariable();
        tracker.startTracking();
        if(checkLoggedIn()){
            handlerLogin = new Handler(Looper.getMainLooper()){
                @Override
                public void handleMessage(Message msg) {
                    super.handleMessage(msg);
                    if(msg != null){
                        tracker.stopTracking();
                        MainActivity.writeLogVerbose("calling needs: " + tracker.getExecutionTime());
                        Intent intent = new Intent(LoginActivity.this, MainActivity.class);
                        intent.putExtra("jsonUser", msg.obj.toString());
                        startActivity(intent);
                        finish();
                        MainActivity.writeLogVerbose("end");
                    }
                    else{
                        setContentView(R.layout.activity_login);
                        editTextEmail = findViewById(R.id.edit_text_email);
                        editTextPassword = findViewById(R.id.edit_text_password);
                        textViewSignUp = findViewById(R.id.text_view_sign_up);
                        buttonLogin = findViewById(R.id.button_login);
                        buttonLogin.setOnClickListener(v -> login());
                        textViewSignUp.setOnClickListener(v -> {
                            getSupportFragmentManager()
                                    .beginTransaction()
                                    .add(R.id.fragment_container, new SignUpFragment())
                                    .addToBackStack(null)
                                    .commit();
                        });
                        editTextEmail.setText(email);
                        editTextPassword.setText(password);
                    }
                }
            };
            email = internalFileReader.readConfiguration("email");
            password = internalFileReader.readConfiguration("password");
            internalFileReader.close();
            String json = jsonBuilder.buildLoginJson(email, password);
            apiCaller.setProperties(handlerLogin, APICaller.RequestMethod.POST, Route.urlLogin, json);

            tracker.stopTracking();
            MainActivity.writeLogVerbose("prepare needs: " + tracker.getExecutionTime());
            tracker.startTracking();
            singleWorker.execute(apiCaller);
            tracker.stopTracking();
            MainActivity.writeLogVerbose("execution needs: " + tracker.getExecutionTime());
            tracker.startTracking();
        }
        else{
            handlerLogin = new Handler(Looper.getMainLooper()){
                @Override
                public void handleMessage(Message msg) {
                    super.handleMessage(msg);
                    if(msg != null){
                        Intent intent = new Intent(LoginActivity.this, MainActivity.class);
                        intent.putExtra("jsonUser", msg.obj.toString());
                        startActivity(intent);
                        finish();
                        Data.writeUserData(LoginActivity.this, editTextEmail.getText().toString(), editTextPassword.getText().toString());
                    }
                    else{
                        Toast.makeText(LoginActivity.this, "Doesn't exist", Toast.LENGTH_LONG).show();
                    }
                }
            };
            setContentView(R.layout.activity_login);
            editTextEmail = findViewById(R.id.edit_text_email);
            editTextPassword = findViewById(R.id.edit_text_password);
            textViewSignUp = findViewById(R.id.text_view_sign_up);
            buttonLogin = findViewById(R.id.button_login);
            buttonLogin.setOnClickListener(v -> login());
            textViewSignUp.setOnClickListener(v -> {
                getSupportFragmentManager()
                        .beginTransaction()
                        .add(R.id.fragment_container, new SignUpFragment())
                        .addToBackStack(null)
                        .commit();
            });
        }
    }
    private boolean checkLoggedIn(){
        return internalFileReader.exists();
    }
    private void login(){
        email = editTextEmail.getText().toString();
        password = editTextPassword.getText().toString();
        String json = jsonBuilder.buildLoginJson(email, password);
        apiCaller.setProperties(handlerLogin, APICaller.RequestMethod.POST, Route.urlLogin, json);
        singleWorker.execute(apiCaller);
    }
    private void initializeGlobalVariable(){
        singleWorker = new SingleWorker(MainActivity.LOG_TAG);
        apiCaller = new APICaller();
        jsonBuilder = new JsonBuilder();
        internalFileReader = new InternalFileReader(this, Data.INTERNAL_ACCOUNT_DIRECTORY, Data.INTERNAL_ACCOUNT_FILE);
        internalFileReader.setConfigurationSeparator(":");
    }
}