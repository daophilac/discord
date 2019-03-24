package com.daophilac.discord.models;
enum Gender{
    Male, Female
}
public class User {
    private int userID;
    private String email;
    private String password;
    private String userName;
    private String fistName;
    private String lastName;
    private Gender gender;
    private String image;

    public int getUserID() {
        return userID;
    }

    public void setUserID(int userID) {
        this.userID = userID;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public String getFirstName() {
        return fistName;
    }

    public void setFirstName(String firstName) {
        fistName = firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public com.daophilac.discord.models.Gender getGender() {
        return gender;
    }

    public void setGender(com.daophilac.discord.models.Gender gender) {
        this.gender = gender;
    }

    public String getImage() {
        return image;
    }

    public void setImage(String image) {
        this.image = image;
    }
}
