package com.peanut.discord.worker;

import android.os.Handler;
import android.os.Looper;
import android.os.Message;

public class ViewManager{
    private String name;
    private SingleWorker singleWorker;
    private MultipleWorker multipleWorker;
    private Runnable taskFindView;
    private Runnable taskDecorateViews;
    private Runnable taskRegisterListeners;
    private Handler handlerDecorateViews;
    private Handler handlerRegisterListeners;
    public ViewManager(String name, Runnable taskFindView, Runnable taskDecorateViews, Runnable taskRegisterListeners){
        this.name = name;
        if(taskFindView != null){
            this.singleWorker = new SingleWorker(this.name);
            this.taskFindView = taskFindView;
        }
        if(taskDecorateViews != null || taskRegisterListeners != null){
            this.multipleWorker = new MultipleWorker(this.name);
            if(taskDecorateViews != null){
                this.handlerDecorateViews = new Handler(Looper.getMainLooper()){
                    @Override
                    public void handleMessage(Message msg) {
                        taskDecorateViews.run();
                    }
                };
                this.multipleWorker.addMoreWorker(1);
                this.taskDecorateViews = taskDecorateViews;
            }
            if(taskRegisterListeners != null){
                this.handlerRegisterListeners = new Handler(Looper.getMainLooper()){
                    @Override
                    public void handleMessage(Message msg) {

                    }
                };
                this.multipleWorker.addMoreWorker(1);
                this.taskRegisterListeners = taskRegisterListeners;
            }
        }
    }
    public void start(){
        if(this.taskFindView != null){
            this.singleWorker.execute(this.taskFindView);
        }
        if(this.taskDecorateViews != null){
            Message message = Message.obtain(this.handlerDecorateViews, this.taskDecorateViews);
            message.sendToTarget();
        }
        if(this.taskRegisterListeners != null){
//            Message message = Message.obtain(this.handlerDecorateViews, this.taskRegisterListeners);
//            message.sendToTarget();
            this.multipleWorker.execute(this.taskRegisterListeners);
        }
    }
}
